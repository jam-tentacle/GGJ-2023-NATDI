public class ResultService : Service, IStart, IUpdate
{
    private AssetsCollection _assetsCollection;
    private UIService _uiService;
    private MushroomerSpawner _mushroomerSpawner;
    private CollectionService _collectionService;

    private bool _isEnd;

    public void GameStart()
    {
        _mushroomerSpawner = Services.Get<MushroomerSpawner>();
        _collectionService = Services.Get<CollectionService>();
        _assetsCollection = Services.Get<AssetsCollection>();
        _uiService = Services.Get<UIService>();

        _uiService.WinUI.SetActive(false);
        _uiService.LoseUI.SetActive(false);
    }

    public void GameUpdate(float delta)
    {
        if (_isEnd)
        {
            return;
        }

        if (TryWin())
        {
            return;
        }

        if (TryLose())
        {
            return;
        }
    }

    private bool TryWin()
    {
        if (_mushroomerSpawner.HasNextWave)
        {
            return false;
        }

        if (_collectionService.GetMushroomerCount > 0)
        {
            return false;
        }

        _isEnd = true;
        _uiService.WinUI.SetActive(true);

        return true;
    }

    private bool TryLose()
    {
        if (_assetsCollection.MainMushroomArea.HasMushrooms)
        {
            return false;
        }

        _isEnd = true;
        _uiService.LoseUI.SetActive(true);
        return true;
    }
}
