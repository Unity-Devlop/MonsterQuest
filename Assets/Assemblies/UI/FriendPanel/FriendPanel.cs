using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityToolkit;

namespace Game.UI
{
    public class FriendPanel : UIPanel
    {
        private FriendListPage _listPage;
        private FriendSearchPage _searchPage;

        private void Awake()
        {
            // TODO
            _searchPage = transform.Find("SearchPage").GetComponent<FriendSearchPage>();
            _listPage = transform.Find("ListPage").GetComponent<FriendListPage>();
        }
        

        public override void OnOpened()
        {
            base.OnOpened();
           _listPage.Open();
           Player.Local.DisableInput();
        }
        
        public override void OnClosed()
        {
            base.OnClosed();
            _listPage.Close();
            Player.Local.EnableInput();
        }
    }
}