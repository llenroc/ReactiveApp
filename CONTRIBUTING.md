## The short version

1. Open `ReactiveApp.sln` in VS2013.
1. Run tests, build ReactiveUI.sln in VS2013 on Win8.1 and make sure everything works.
1. Make chnges and submit a PR.


## The longer version to contribute to ReactiveApp

1. Fork and Clone the source.
1. Create a new branch for your feature or bugfix.
1. Open the ReactiveApp.sln solution in VS2013. You might get errors if you do not have the Xamarin tools. 
1. Run all the tests, make sure they pass.
1. Write some new tests that fail.
1. Make your change.
1. Make those new tests pass.
1. Push that branch to GitHub (`git push -u origin my-cool-new-feature`)
1. Go to your fork on GitHub, you should see a button with your branch next to it labeled 'Pull Request'
1. Type up a clear description of why we need this change and what it is changing.

## Some remarks

* Make sure you have `Place system directives first when sorting usings` is checked in `Options -> Text Editor -> C# -> Advanced` 
* Make sure your tab and indent sizes are `4` and that `insert spaces` is checked in `Options -> Text Editor -> C# -> Tabs`
* Follow the default coding conventions of Visual Studio.
* Never click `Enable NuGet package restore`. We use Automatic package restore which is part of NuGet 2.7 and higher.
