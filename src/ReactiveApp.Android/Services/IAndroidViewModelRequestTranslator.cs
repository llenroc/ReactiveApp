using Android.Content;
using ReactiveApp.ViewModels;

namespace ReactiveApp.Android.Services
{
    public interface IAndroidViewModelRequestTranslator
    {
        Intent GetIntentForViewModelRequest(ReactiveViewModelRequest viewModelRequest);
    }
}