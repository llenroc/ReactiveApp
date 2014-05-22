using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Windows;
using ReactiveUI;
using TestApp.Models;

namespace TestApp.ViewModels
{
    public class GroupViewModel : ReactiveObject
    {
        private readonly SampleDataGroup group;

        public GroupViewModel(SampleDataGroup group)
        {
            this.group = group;

            this.Items = new ReactiveList<ItemViewModel>(group.Items.Select(i => new ItemViewModel(i)));
        }

        public string UniqueId { get { return group.UniqueId; } }

        public string Title { get { return group.Title; } }

        public string Subtitle { get { return group.Subtitle; } }

        public string Description { get { return group.Description; } }

        public string ImagePath { get { return group.ImagePath; } }

        public ReactiveList<ItemViewModel> Items { get; private set; }
    }
}