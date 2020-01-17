using Android.Content;

namespace Zebra.Savanna.Sample
{
    public interface IScanReceiver
    {
        void DisplayScanResult(Intent intent);
    }
}