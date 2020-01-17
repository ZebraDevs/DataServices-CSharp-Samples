using Android.Content;

namespace Zebra.Savanna.Sample
{
    public class SavannaBroadcastReceiver : BroadcastReceiver
    {
        IScanReceiver _activity;
        public SavannaBroadcastReceiver(IScanReceiver activity)
        {
            _activity = activity;
        }

        public override void OnReceive(Context context, Intent intent)
        {
            string action = intent.Action;
            //Bundle b = intent.getExtras();

            //  This is useful for debugging to verify the format of received intents from DataWedge
            //for (String key : b.keySet())
            //{
            //    Log.v(LOG_TAG, key);
            //}

            if (action != null && action.Equals(context.Resources.GetString(Resource.String.activity_intent_filter_action)))
            {
                //  Received a barcode scan
                try
                {
                    _activity.DisplayScanResult(intent);
                }
                catch
                {
                    //  Catch if the UI does not exist when we receive the broadcast
                }
            }
        }
    }
}