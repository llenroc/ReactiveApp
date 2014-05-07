using Android.Content;
using ReactiveApp.ViewModels;

namespace ReactiveApp.Android.Services
{
    public interface IAndroidReactiveViewModelRequestTranslator
    {
        Intent GetIntentForViewModelRequest(ReactiveViewModelRequest viewModelRequest);

        ReactiveViewModelRequest GetViewModelRequestForIntent(Intent intent);
    }
}