using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using Reactive.Bindings;
using System.Threading.Tasks;
using Sample.Views;
using System.ComponentModel;
using Xamarin.Forms;
using AiForms.Dialogs;
using AiForms.Dialogs.Abstractions;
using Prism.Unity;

namespace Sample.ViewModels
{
    public class MainPageViewModel : BindableBase, INavigationAware
    {
        public AsyncReactiveCommand LoadingCommand { get; } = new AsyncReactiveCommand();
        public AsyncReactiveCommand CustomLoadingCommand { get; set; } = new AsyncReactiveCommand();
        public ReactiveCommand DialogCommand { get; } = new ReactiveCommand();
        public ReactiveCommand ToastCommand { get; } = new ReactiveCommand();
        public ReactiveCommand StartCommand { get; set; } = new ReactiveCommand();

        public List<LayoutAlignment> VAligns { get; set; } = new List<LayoutAlignment>();
        public List<LayoutAlignment> HAligns { get; set; } = new List<LayoutAlignment>();

        public ReactivePropertySlim<int> OffsetX { get; } = new ReactivePropertySlim<int>();
        public ReactivePropertySlim<int> OffsetY { get; } = new ReactivePropertySlim<int>();
        public ReactivePropertySlim<LayoutAlignment> VAlign { get; } = new ReactivePropertySlim<LayoutAlignment>();
        public ReactivePropertySlim<LayoutAlignment> HAlign { get; } = new ReactivePropertySlim<LayoutAlignment>();
        public ReactivePropertySlim<bool> UseCurrentPageLocation { get; } = new ReactivePropertySlim<bool>(false);
        private readonly INavigationService navigationService;

        public MainPageViewModel(MyIndicatorView myIndicatorView, INavigationService navigationService)
        {

            VAligns.Add(LayoutAlignment.Start);
            VAligns.Add(LayoutAlignment.Center);
            VAligns.Add(LayoutAlignment.End);

            HAligns.Add(LayoutAlignment.Start);
            HAligns.Add(LayoutAlignment.Center);
            HAligns.Add(LayoutAlignment.End);

            VAlign.Value = VAligns[1];
            HAlign.Value = HAligns[1];

            Configurations.LoadingConfig = new LoadingConfig{DefaultMessage = "Loading...",IsReusable = true};

            var loadingFlg = false;
            LoadingCommand.Subscribe(async _ =>
            {
                //Loading.Instance.Show();
                //await Task.Delay(1);
                //Loading.Instance.Hide();

                await navigationService.GoBackAsync(useModalNavigation: true);
                return;

                await Loading.Instance.StartAsync(async progress => {
                    //await Task.Delay(1);
                    progress.Report(0d);
                    for (var i = 0; i < 100; i++)
                    {
                        if (i == 50)
                        {
                            Loading.Instance.SetMessage("Soon...");
                        }
                        await Task.Delay(25);
                        progress.Report((i + 1) * 0.01d);
                    }
                },null,loadingFlg).ConfigureAwait(false);

                loadingFlg = !loadingFlg;
            });


            CustomLoadingCommand.Subscribe(async _ =>
            {
                if (_customLoading == null)
                {
                    _customLoading = Loading.Instance.Create<MyIndicatorView>(new
                    {
                        Message = "Loading...",
                        VAlign = VAlign.Value,
                        HAlign = HAlign.Value,
                        OffsetX = OffsetX.Value,
                        OffsetY = OffsetY.Value
                    });
                }



                _customLoading.Show();
                await Task.Delay(10000);
                _customLoading.Hide();

            });

            var dlgPage = new MyDialogView();


            DialogCommand.Subscribe(async _ =>
            {


                //var ret = await redlg.ShowAsync();

                //dlg.Dispose();
                var vmm = new { 
                    Title = "Title", Description = "This is a forms view.",
                    VAlign = VAlign.Value,
                    HAlign = HAlign.Value,
                    OffsetX = OffsetX.Value,
                    OffsetY = OffsetY.Value,
                    IsPageLocation = UseCurrentPageLocation.Value,
                };

                var ret = await Dialog.Instance.ShowAsync<MyDialogView>(vmm);
                //var ret = await Dialog.Instance.ShowAsync(page, vm);
            });

            ToastCommand.Subscribe(_ =>
            {
                Toast.Instance.Show<MyToastView>(new {
                    VAlign = VAlign.Value, HAlign = HAlign.Value,OffsetX = OffsetX.Value, OffsetY = OffsetY.Value 
                });
            });

		}

		public void OnNavigatedFrom(NavigationParameters parameters)
		{

		}

        IReusableDialog redlg;
        private IReusableLoading _customLoading;

        public void OnNavigatedTo(NavigationParameters parameters)
		{
            var vm = new { Title = "Title", Description = "Some description write here." };
            redlg = Dialog.Instance.Create<MyDialogView>(vm);
        }

		public void OnNavigatingTo(NavigationParameters parameters)
		{
		}
	}
}

