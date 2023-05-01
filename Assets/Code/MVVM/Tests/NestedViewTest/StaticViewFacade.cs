using System;
using DM.MVVM.View;
using UnityEngine;

public class StaticViewFacade : ViewFacade
{
	#region Properties
	[field : SerializeField]
	public ItemViewFacade ItemOne { get; private set; }

	[field : SerializeField]
	public ItemViewFacade ItemTwo { get; private set; }

	[field : SerializeField]
	public ItemViewFacade ItemThree { get; private set; }
	#endregion
	
	
	
}