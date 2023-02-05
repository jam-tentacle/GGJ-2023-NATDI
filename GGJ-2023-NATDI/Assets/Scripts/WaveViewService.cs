
public class WaveViewService : Service, IStart, IUpdate
{
    private MushroomerSpawner _mushroomerSpawner;
    private UIService _uiService;
    private CollectionService _collectionService;

    public void GameStart()
    {
        _mushroomerSpawner = Services.Get<MushroomerSpawner>();
        _uiService = Services.Get<UIService>();
        _collectionService = Services.Get<CollectionService>();
    }

    public void GameUpdate(float delta)
    {
        _uiService.WaveStationText.text = $"{_mushroomerSpawner.LeftWaves}";
        _uiService.WaveUI.SetContent(_mushroomerSpawner.HasNextWave, (int)_mushroomerSpawner.GetLeftNextWaveTime(), _mushroomerSpawner.LeftWaves, _collectionService.GetMushroomerCount);
    }
}
