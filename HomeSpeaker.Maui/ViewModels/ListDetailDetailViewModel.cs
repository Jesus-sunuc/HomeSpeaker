﻿namespace HomeSpeaker.Maui.ViewModels;

[QueryProperty(nameof(Item), "Item")]
public partial class ListDetailDetailViewModel : BaseViewModel
{
    [ObservableProperty]
    SampleItem item;
}
