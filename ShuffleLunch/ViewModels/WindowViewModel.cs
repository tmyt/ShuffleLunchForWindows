﻿using lunch_proto.Utils;
using ShuffleLunch.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace ShuffleLunch.ViewModels
{
	class WindowViewModel : INotifyPropertyChanged
	{
		#region プロパティ変更通知

		// INotifyPropertyChanged
		public event PropertyChangedEventHandler PropertyChanged;

		protected bool SetProperty<T>(ref T storage, T value, [CallerMemberName] String propertyName = null)
		{
			if (object.Equals(storage, value)) return false;

			storage = value;
			this.OnPropertyChanged(propertyName);
			return true;
		}

		protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			var eventHandler = this.PropertyChanged;
			if (eventHandler != null)
			{
				eventHandler(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		#endregion

		#region Title 変更通知プロパティ

		private string _title;

		public string Title
		{
			get { return this._title; }
			set
			{
				SetProperty(ref _title, value);
			}
		}

		#endregion

		#region DeskList 変更通知プロパティ

		private ObservableCollection<Desk> _deskList = new ObservableCollection<Desk>();
		public ObservableCollection<Desk> DeskList
		{
			get { return _deskList; }
			set
			{
				SetProperty(ref _deskList, value);
			}
		}

		#endregion

		#region PersonList 変更通知プロパティ

		private ObservableCollection<Person> _personList = new ObservableCollection<Person>();
		public ObservableCollection<Person> PersonList
		{
			get { return _personList; }
			set
			{
				SetProperty(ref _personList, value);
			}
		}

		#endregion

		#region PersonAndDeskList 変更通知プロパティ

		private ObservableCollection<PersonAndDesk> _personAndDeskList = new ObservableCollection<PersonAndDesk>();
		public ObservableCollection<PersonAndDesk> PersonAndDeskList
		{
			get { return _personAndDeskList; }
			set
			{
				SetProperty(ref _personAndDeskList, value);
			}
		}

		#endregion

		#region ShuffleResultList 変更通知プロパティ

		private ObservableCollection<ShuffleResult> _shuffleResultList = new ObservableCollection<ShuffleResult>();
		public ObservableCollection<ShuffleResult> ShuffleResultList
		{
			get { return _shuffleResultList; }
			set
			{
				SetProperty(ref _shuffleResultList, value);
			}
		}

		#endregion

		#region AddUserName 変更通知プロパティ

		private string _addUserName;
		public string AddUserName
		{
			get { return _addUserName; }
			set
			{
				SetProperty(ref _addUserName, value);
			}
		}

		#endregion

		private LunchInfo _lunchInfo;

		/// <summary>
		/// ファイルオープン
		/// </summary>
		public ICommand FileOpen { get; private set; }

		/// <summary>
		/// 参加者をシャッフル
		/// </summary>
		public ICommand ButtonShuffle { get; private set; }

		/// <summary>
		/// 参加者追加
		/// </summary>
		public ICommand ButtonAddUser { get; private set; }

        public ICommand ExportImage { get; private set; }

		public WindowViewModel()
		{
			Title = "ShuffleLunch";

			_lunchInfo = new LunchInfo();

			FileOpen = new DelegateCommand(_ =>
			{
				var b = _lunchInfo.Get();
				if (b == false)
				{
					return;
				}

				PersonList = new ObservableCollection<Person>(_lunchInfo.PersonList());
				DeskList = new ObservableCollection<Desk>(_lunchInfo.DeskList());
				PersonAndDeskList = new ObservableCollection<PersonAndDesk>(_lunchInfo.PersonAndDeskList());

			});

			ButtonShuffle = new DelegateCommand(_ =>
			{
				var shuffle = new Shuffle();
				var b = shuffle.shuffle(DeskList.ToList<Desk>(), PersonAndDeskList.ToList<PersonAndDesk>());
				if (b == false)
				{
					return;
				}

				ShuffleResultList = new ObservableCollection<ShuffleResult>(shuffle.Get());

			});

			ButtonAddUser = new DelegateCommand(_ =>
			{
				var deskList = new List<string>();
				for (int i = 0; i < DeskList.Count; i++)
				{
					deskList.Add(DeskList[i].name);
				}
				var personAndDesk = new PersonAndDesk
				{
					name = AddUserName,
					desk = deskList,
					selectDesk = 0,
					image = @""
				};
				PersonAndDeskList.Add(personAndDesk);
				AddUserName = "";
			});

            ExportImage = new DelegateCommand(element =>
            {
                PngExporter.Export((FrameworkElement)element);
            });
		}

	}
}
