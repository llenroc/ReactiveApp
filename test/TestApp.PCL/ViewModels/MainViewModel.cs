using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using ReactiveApp;
using ReactiveApp.ViewModels;
using ReactiveUI;
using TestApp.Models;
using TestApp.Services;

namespace TestApp.ViewModels
{
    public class MainViewModel : ReactiveViewModel
    {
        private readonly ISampleDataService sampleDataService;

        public MainViewModel(ISampleDataService sampleDataService)
        {
            this.sampleDataService = sampleDataService;

            this.WhenActivatedWithState((param, state, d) =>
            {
                d(sampleDataService.GetSampleDataAsync()
                    .ToObservable()
                    .Select(root => root.Groups.Select(g => new GroupViewModel(g)))
                    .Select(e => new ReactiveList<GroupViewModel>(e))
                    .ToProperty(this, x => x.Groups, out this.groups, null, RxApp.MainThreadScheduler));

                d(this.WhenAnyValue(x => x.Groups).Where(x => x != null).Select(g => g[0]).ToProperty(this, x => x.FirstGroup, out this.firstGroup, null, RxApp.MainThreadScheduler));
                d(this.WhenAnyValue(x => x.Groups).Where(x => x != null).Select(g => g[1]).ToProperty(this, x => x.SecondGroup, out this.secondGroup, null, RxApp.MainThreadScheduler));
                d(this.WhenAnyValue(x => x.Groups).Where(x => x != null).Select(g => g[2]).ToProperty(this, x => x.ThirdGroup, out this.thirdGroup, null, RxApp.MainThreadScheduler));
                d(this.WhenAnyValue(x => x.Groups).Where(x => x != null).Select(g => g[3]).ToProperty(this, x => x.FourthGroup, out this.fourthGroup, null, RxApp.MainThreadScheduler));
            });
        }

        private ObservableAsPropertyHelper<ReactiveList<GroupViewModel>> groups;
        /// <summary>
        /// A collection for ItemViewModel objects.
        /// </summary>
        public ReactiveList<GroupViewModel> Groups
        {
            get { return groups != null ? groups.Value : null; }
        }

        private ObservableAsPropertyHelper<GroupViewModel> firstGroup;
        public GroupViewModel FirstGroup
        {
            get { return firstGroup != null ? firstGroup.Value : null; }
        }

        private ObservableAsPropertyHelper<GroupViewModel> secondGroup;
        public GroupViewModel SecondGroup
        {
            get { return secondGroup != null ? secondGroup.Value : null; }
        }

        private ObservableAsPropertyHelper<GroupViewModel> thirdGroup;
        public GroupViewModel ThirdGroup
        {
            get { return thirdGroup != null ? thirdGroup.Value : null; }
        }

        private ObservableAsPropertyHelper<GroupViewModel> fourthGroup;
        public GroupViewModel FourthGroup
        {
            get { return fourthGroup != null ? fourthGroup.Value : null; }
        }
    }
}