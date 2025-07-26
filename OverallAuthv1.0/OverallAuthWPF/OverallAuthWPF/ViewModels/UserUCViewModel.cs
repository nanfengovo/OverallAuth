using OverallAuthWPF.APIHelper;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using OverallAuthWPF.APIHelper.Model;
using System.Threading;
using MvvmHelpers.Commands;
using Prism.Regions;

namespace OveralllAuth_V1.ViewModels
{
    public class UserUCViewModel : BindableBase,INavigationAware
    {
        private readonly ApiService _apiService;

        // 使用 ObservableCollection 支持动态更新 UI 列表
        private List<User> _users;
        public List<User> Users
        {
            get => _users;
            set => SetProperty(ref _users, value);
        }

        // 加载状态（用于控制进度条）
        private bool _isLoading;
        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        // 错误信息
        private string _errorMessage;
        public string ErrorMessage
        {
            get => _errorMessage;
            set => SetProperty(ref _errorMessage, value);
        }

        // 异步加载命令（支持取消）
        public AsyncCommand LoadDataCommand { get; }

        public UserUCViewModel(ApiService apiService)
        {
            _apiService = apiService;
            Users = new List<User>();
            LoadDataCommand = new AsyncCommand(LoadDataAsync);
        }

        private async Task LoadDataAsync()
        {
            try
            {
                IsLoading = true;
                ErrorMessage = null;

                // 调用 API 获取数据
                var users = await _apiService.GetDataAsync();

                // 清空旧数据并填充新数据
                //Users.Clear();
                //foreach (var user in users)
                //{
                //    Users.Add(user);
                //}
                Users = users;
            }
            catch (OperationCanceledException)
            {
                // 取消操作无需处理
            }
            catch (Exception ex)
            {
                ErrorMessage = $"加载失败: {ex.Message}";
            }
            finally
            {
                IsLoading = false;
            }
        }

        public async void OnNavigatedTo(NavigationContext navigationContext)
        {
            // 路由切换后触发数据加载
            await LoadDataAsync();
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            throw new NotImplementedException();
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
            throw new NotImplementedException();
        }
    }
}
