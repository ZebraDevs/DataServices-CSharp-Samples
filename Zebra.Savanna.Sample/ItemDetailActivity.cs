using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using AndroidX.AppCompat.App;
using AndroidX.AppCompat.Widget;
using System;

namespace Zebra.Savanna.Sample
{
    /// <summary>
    /// An activity representing a single Item detail screen. This
    /// activity is only used on narrow width devices. On tablet-size devices,
    /// item details are presented side-by-side with a list of items
    /// in a <see cref="ListItemActivity" />
    /// </summary>
    [Activity(Name = "com.zebra.savanna.sample.ItemDetailActivity",
        ParentActivity = typeof(ItemListActivity),
        Theme = "@style/AppTheme.NoActionBar")]
    [MetaData("android.support.PARENT_ACTIVITY",
        Value = "com.zebra.savanna.sample.ItemListActivity")]
    public class ItemDetailActivity : AppCompatActivity, IScanReceiver
    {
        //
        // After registering the broadcast receiver, the next step (below) is to define it.
        // Here it's done in the MainActivity.cs, but also can be handled by a separate class.
        // The logic of extracting the scanned data and displaying it on the screen
        // is executed in its own method (later in the code). Note the use of the
        // extra keys defined in the strings.xml file.
        //
        private readonly BroadcastReceiver _broadcastReceiver;

        public ItemDetailActivity()
        {
            _broadcastReceiver = new SavannaBroadcastReceiver(this);
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_item_detail);
            var toolbar = FindViewById<Toolbar>(Resource.Id.detail_toolbar);
            SetSupportActionBar(toolbar);

            // Show the Up button in the action bar.
            SupportActionBar?.SetDisplayHomeAsUpEnabled(true);

            // savedInstanceState is non-null when there is fragment state
            // saved from previous configurations of this activity
            // (e.g. when rotating the screen from portrait to landscape).
            // In this case, the fragment will automatically be re-added
            // to its container so we don't need to manually add it.
            // For more information, see the Fragments API guide at:
            //
            // http://developer.android.com/guide/components/fragments.html
            //
            if (savedInstanceState == null)
            {
                // Create the detail fragment and add it to the activity
                // using a fragment transaction.
                Bundle arguments = new Bundle();
                arguments.PutString(ItemDetailFragment.ArgItemId,
                        Intent.GetStringExtra(ItemDetailFragment.ArgItemId));
                ItemDetailFragment fragment = new ItemDetailFragment
                {
                    Arguments = arguments
                };
                SupportFragmentManager.BeginTransaction()
                        .Add(Resource.Id.item_detail_container, fragment)
                        .Commit();
            }
            IntentFilter filter = new IntentFilter();
            filter.AddCategory(Intent.CategoryDefault);
            filter.AddAction(Resources.GetString(Resource.String.activity_intent_filter_action));
            RegisterReceiver(_broadcastReceiver, filter);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            UnregisterReceiver(_broadcastReceiver);
        }

        //
        // The section below assumes that a UI exists in which to place the data. A production
        // application would be driving much of the behavior following a scan.
        //
        public void DisplayScanResult(Intent initiatingIntent)
        {
            string decodedData = initiatingIntent.GetStringExtra(Resources.GetString(Resource.String.datawedge_intent_key_data));
            string decodedLabelType = initiatingIntent.GetStringExtra(Resources.GetString(Resource.String.datawedge_intent_key_label_type)).ToLower();

            Console.WriteLine("decodedData: " + decodedData);
            Console.WriteLine("decodedLabelType: " + decodedLabelType);

            ItemDetailFragment.Instance.RouteScanData(decodedData, decodedLabelType);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            int id = item.ItemId;
            if (id == Android.Resource.Id.Home)
            {
                // This ID represents the Home or Up button. In the case of this
                // activity, the Up button is shown. For
                // more details, see the Navigation pattern on Android Design:
                //
                // http://developer.android.com/design/patterns/navigation.html#up-vs-back
                //
                NavigateUpTo(new Intent(this, typeof(ItemListActivity)));
                return true;
            }
            return base.OnOptionsItemSelected(item);
        }
    }
}