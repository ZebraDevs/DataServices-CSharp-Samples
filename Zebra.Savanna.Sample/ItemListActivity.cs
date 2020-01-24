using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.App;
using AndroidX.RecyclerView.Widget;
using Zebra.Savanna.Sample.API;
using System;
using System.Collections.Generic;

namespace Zebra.Savanna.Sample
{
    /// <summary>
    /// An activity representing a list of Items. This activity
    /// has different presentations for handset and tablet-size devices. On
    /// handsets, the activity presents a list of items, which when touched,
    /// lead to a <see cref="ItemDetailActivity"/> representing
    /// item details. On tablets, the activity presents the list of items and
    /// item details side-by-side using two vertical panes.
    /// </summary>
    [Activity(MainLauncher = true,
        Name = "com.zebra.barcodeintelligencetools.ItemListActivity",
        Label = "@string/app_name",
        Theme = "@style/AppTheme.NoActionBar")]
    public class ItemListActivity : AppCompatActivity, IScanReceiver
    {
        public static int Density { get; set; }
        internal static APIContent _content;

        /// <summary>
        /// Whether or not the activity is in two-pane mode, i.e. running on a tablet device.
        /// </summary>
        private bool _twoPane;
        //
        // After registering the broadcast receiver, the next step (below) is to define it.
        // Here it's done in the MainActivity.cs, but also can be handled by a separate class.
        // The logic of extracting the scanned data and displaying it on the screen
        // is executed in its own method (later in the code). Note the use of the
        // extra keys defined in the strings.xml file.
        //
        private readonly BroadcastReceiver _broadcastReceiver;

        public ItemListActivity()
        {
            _broadcastReceiver = new SavannaBroadcastReceiver(this);
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_item_list);

            if (_content == null)
            {
                _content = new APIContent();
            }
            else
            {
                _content.Clear();
            }
            _content.AddItem(GetString(Resource.String.create_barcode), GetString(Resource.String.create_barcode_details), Resource.Drawable.ic_createbarcode);
            _content.AddItem(GetString(Resource.String.fda_recall), GetString(Resource.String.fda_recall_details), Resource.Drawable.ic_fdarecall);
            _content.AddItem(GetString(Resource.String.upc_lookup), GetString(Resource.String.upc_lookup_details), Resource.Drawable.ic_upclookup);

            var toolbar = FindViewById<AndroidX.AppCompat.Widget.Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);
            toolbar.Title = Title;

            if (FindViewById(Resource.Id.item_detail_container) != null)
            {
                // The detail container view will be present only in the
                // large-screen layouts (res/values-w900dp).
                // If this view is present, then the
                // activity should be in two-pane mode.
                _twoPane = true;
            }

            View recyclerView = FindViewById(Resource.Id.item_list);
            SetupRecyclerView((RecyclerView)recyclerView);
            IntentFilter filter = new IntentFilter();
            filter.AddCategory(Intent.CategoryDefault);
            filter.AddAction(Resources.GetString(Resource.String.activity_intent_filter_action));
            RegisterReceiver(_broadcastReceiver, filter);
            Density = (int)Math.Ceiling(Resources.DisplayMetrics.Density);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            if (item.ItemId == Resource.Id.settings)
            {
                Intent intent = new Intent(this, typeof(SettingsActivity));
                StartActivity(intent);
                return true;
            }
            return base.OnOptionsItemSelected(item);
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
            if (!_twoPane) return;
            string decodedData = initiatingIntent.GetStringExtra(Resources.GetString(Resource.String.datawedge_intent_key_data));
            string decodedLabelType = initiatingIntent.GetStringExtra(Resources.GetString(Resource.String.datawedge_intent_key_label_type)).ToLower();

            Console.WriteLine("decodedData: " + decodedData);
            Console.WriteLine("decodedLabelType: " + decodedLabelType);

            ItemDetailFragment.Instance.RouteScanData(decodedData, decodedLabelType);
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater inflater = MenuInflater;
            inflater.Inflate(Resource.Menu.main, menu);
            return true;
        }

        private void SetupRecyclerView(RecyclerView recyclerView)
        {
            var items = new List<ApiItem>(_content.Values);
            recyclerView.SetAdapter(new SimpleItemRecyclerViewAdapter(this, items, _twoPane));
        }

        class SimpleItemRecyclerViewAdapter : RecyclerView.Adapter, View.IOnClickListener
        {
            private readonly ItemListActivity _parentActivity;
            private readonly List<ApiItem> _values;
            private readonly bool _twoPane;

            public void OnClick(View view)
            {
                ApiItem item = (ApiItem)view.Tag;
                if (_twoPane)
                {
                    Bundle arguments = new Bundle();
                    arguments.PutString(ItemDetailFragment.ArgItemId, item.Id);
                    ItemDetailFragment fragment = new ItemDetailFragment
                    {
                        Arguments = arguments
                    };
                    _parentActivity.SupportFragmentManager.BeginTransaction()
                            .Replace(Resource.Id.item_detail_container, fragment)
                            .Commit();
                }
                else
                {
                    Context context = view.Context;
                    Intent intent = new Intent(context, typeof(ItemDetailActivity));
                    intent.PutExtra(ItemDetailFragment.ArgItemId, item.Id);

                    context.StartActivity(intent);
                }
            }

            public SimpleItemRecyclerViewAdapter(ItemListActivity parent, List<ApiItem> items, bool twoPane)
            {
                _values = items;
                _parentActivity = parent;
                _twoPane = twoPane;
            }

            public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
            {
                var view = LayoutInflater.From(parent.Context)
                    .Inflate(Resource.Layout.item_list_content, parent, false);
                return new ViewHolder(view);
            }

            public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
            {
                ((ViewHolder)holder).IconView.SetImageResource(_values[position].Icon);
                ((ViewHolder)holder).ContentView.Text = _values[position].Content;

                holder.ItemView.Tag = _values[position];
                holder.ItemView.SetOnClickListener(this);
            }

            public override int ItemCount { get => _values.Count; }

            public class ViewHolder : RecyclerView.ViewHolder
            {
                public readonly ImageView IconView;
                public readonly TextView ContentView;

                public ViewHolder(View view) : base(view)
                {
                    IconView = view.FindViewById<ImageView>(Resource.Id.icon);
                    ContentView = view.FindViewById<TextView>(Resource.Id.content);
                }
            }
        }
    }
}