using Proto;
using UnityEngine;

namespace Game.UI
{
    public class FriendSearchPage : MonoBehaviour
    {
        private SearchBox _searchBox;
        private FriendSearchResult _searchResult;

        private void Awake()
        {
            _searchBox = transform.Find("SearchBox").GetComponent<SearchBox>();
            _searchResult = transform.Find("SearchResult").GetComponent<FriendSearchResult>();
            _searchResult.Bind(null);
            _searchBox.OnSearch += OnSearch;
        }

        private async void OnSearch(string content)
        {
            _searchResult.Bind(null);
            SearchFriendResponse response = await GrpcClient.GameService.SearchFriendAsync(new SearchFriendRequest
            {
                SearcherUid = Player.Local.userId,
                TargetUid = content
            }).ResponseAsync;

            if (response.Error.Code != StatusCode.Ok)
            {
                Debug.LogError(response.Error.Content);
                return;
            }

            _searchResult.Bind(response);
        }
    }
}