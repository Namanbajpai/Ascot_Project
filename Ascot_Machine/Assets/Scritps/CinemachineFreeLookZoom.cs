
using Cinemachine;
using UnityEngine;

public class CinemachineFreeLookZoom : MonoBehaviour
{
    //[SerializeField] private CinemachineFreeLook freelook;
    [SerializeField] private CinemachineFreeLook _activeFreeLook;
    private CinemachineFreeLook.Orbit[] originalOrbits;
    [Tooltip("The minimum scale for the orbits")]
    [Range(0f, 1f)]
    public float minScale = 0.2f;

    [Tooltip("The maximum scale for the orbits")]
    [Range(1F, 40f)]
    public float maxScale = 1;

    [Tooltip("The zoom axis.  Value is 0..1.  How much to scale the orbits")]
    [AxisStateProperty]
    public AxisState zAxis = new AxisState(0, 1, false, true, 50f, 0.1f, 0.1f, "Mouse ScrollWheel", false);

    private float _scale;
    public float Scale { get { return zAxis.Value; } }

    void OnValidate()
    {
        minScale = Mathf.Max(0.01f, minScale);
        maxScale = Mathf.Max(minScale, maxScale);
    }

    void Awake()
    {
        //freelook = GetComponentInChildren<CinemachineFreeLook>();
        
        if (_activeFreeLook != null)
        {
            originalOrbits = new CinemachineFreeLook.Orbit[_activeFreeLook.m_Orbits.Length];
            for (int i = 0; i < originalOrbits.Length; i++)
            {
                originalOrbits[i].m_Height = _activeFreeLook.m_Orbits[i].m_Height;
                originalOrbits[i].m_Radius = _activeFreeLook.m_Orbits[i].m_Radius;
            }
#if UNITY_EDITOR
            SaveDuringPlay.SaveDuringPlay.OnHotSave -= RestoreOriginalOrbits;
            SaveDuringPlay.SaveDuringPlay.OnHotSave += RestoreOriginalOrbits;
#endif
        }
        
    }

    public void UpdateZoomHandler(CinemachineFreeLook activeFreeLook)
    {
        this._activeFreeLook = activeFreeLook;
        zAxis.Value = 0;
        originalOrbits = new CinemachineFreeLook.Orbit[activeFreeLook.m_Orbits.Length];
        for (int i = 0; i < originalOrbits.Length; i++)
        {
            originalOrbits[i].m_Height = activeFreeLook.m_Orbits[i].m_Height;
            originalOrbits[i].m_Radius = activeFreeLook.m_Orbits[i].m_Radius;
        }
        
    }

#if UNITY_EDITOR
    private void OnDestroy()
    {
        SaveDuringPlay.SaveDuringPlay.OnHotSave -= RestoreOriginalOrbits;
    }

    private void RestoreOriginalOrbits()
    {
        if (originalOrbits != null)
        {
            for (int i = 0; i < originalOrbits.Length; i++)
            {
                _activeFreeLook.m_Orbits[i].m_Height = originalOrbits[i].m_Height;
                _activeFreeLook.m_Orbits[i].m_Radius = originalOrbits[i].m_Radius;
            }
        }
    }
#endif

    void Update()
    {
        
        if (originalOrbits != null)
        {
            zAxis.Update(Time.deltaTime * zAxis.m_MaxSpeed);
            _scale = Mathf.Lerp(minScale, maxScale, zAxis.Value);

            for (int i = 0; i < originalOrbits.Length; i++)
            {
                _activeFreeLook.m_Orbits[i].m_Height = originalOrbits[i].m_Height * _scale;
                _activeFreeLook.m_Orbits[i].m_Radius = originalOrbits[i].m_Radius * _scale;
            }
        }
        
    }
}