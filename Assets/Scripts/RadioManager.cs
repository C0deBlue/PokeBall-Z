using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

// Below comments written by Jonathan Sands 12/17/2022 @ 11:15AM
// ~
// TODO: Make work with Audio System when complete.  Add persistence with File I/O
// Radio shut off sfx
/// <summary>
/// Bare Bones of playing stations on a radio implemented
/// Notes:
/// Currently have 4 audio tracks added to the MasterListOfStations via the inspector
/// Other collections are initialized on program start
/// I arbitrarily add 3 of the stations to the unlockedStations list.  This will likely be driven by a file in the future
/// The radio starts in the 'off' state.  Every time you click it, a static sound plays and the next station in the list plays.
///     The clicking functionality will likely be changed.  Interacting with the radio (press E when in front) could bring up a menu so you can choose
///     which station to listen to and see which are locked/unlocked
/// Code has functionality to unlock a station by name.  Pass in the name of the clip to unlock, and as long as it exists in the master list
/// and hasn't already been unlocked, it will unlock.
/// </summary>
// ~
public class RadioManager : MonoBehaviour
{
    // Master list of ALL the stations (behind the scenes)
    // Used to easily check if a station exists in the masterlist
    private Dictionary<string, AudioClip> DictOfStations;

    // Contains only the stations that have been unlocked
    private List<AudioClip> unlockedStations;

    // Public facing list of ALL stations
    public List<AudioClip> MasterListOfStations;

    // Keeps track of which stations have been unlocked
    public Dictionary<string, bool> MasterKey;

    // PLay static before switching
    public AudioClip radioStatic;

    // NEED: Radio shut off sfx

    public int activeIndex = 0;

    public AudioSource radio;

    public bool isActive;

    public float radioTimer;


    // WIll need a saved key of some sort to keep track of unlocked stations
    // Start is called before the first frame update
    void Start()
    {
        // Initialize collections
        DictOfStations = new Dictionary<string, AudioClip>();
        MasterKey = new Dictionary<string, bool>();
        unlockedStations = new List<AudioClip>();
        isActive = false;

        // populate the ListOfStations dictionary from the MasterListOfStations
        foreach (AudioClip clip in MasterListOfStations)
        {
            DictOfStations.Add(clip.name, clip);        // Use the clip's name as the key
            MasterKey.Add(clip.name, clip);
            MasterKey[clip.name] = false;
        }

        radioTimer = 0;
        
        // first station of unlockStations is always static
        unlockedStations.Add(radioStatic);
        // Next unlock the first 3 radio stations in the master list
        // This is temporary - This will be controlled by a save file of some sort in the future
        for (int i = 0; i < 3; i++)
        {
            // Unlock the station
            UnlockStation(MasterListOfStations[i].name);
        }
        radio.clip = unlockedStations[activeIndex];
    }

    // Update is called once per frame
    void Update()
    {
        radioTimer += Time.deltaTime;
        
        // This is TEMPORARY to show how unlocking a new station would work.
        // In the future, the method will be queried by a separate script and will pass the name to unlock here.
        if (Input.GetKeyDown(KeyCode.U))
        {
            UnlockStation("Track 04");
        }

    }

    // This will occur while changing stations.  Checks if we need to reset the timer
    private void HandleTimer()
    {
        if (radioTimer > 10000)      // if a clip is over 10000 seconds long, god help you
            radioTimer = 0;
    }

    /*
    public void OnPointerClick(PointerEventData eventData)
    {
        // if (eventData.button == PointerEventData.InputButton.Left)
        //     ToggleRadio();
        // if (eventData.button == PointerEventData.InputButton.Right)
        //     CycleStation();
    }
    */

    // Toggles the radio on/off
    /*
    public void ToggleRadio()
    {
        // Check if timer needs to reset
        HandleTimer();
        isActive = !isActive;
        if (isActive)
        {
            // Play where the radio currently is in time
            radio.time = radioTimer % radio.clip.length;
            radio.Play();
        }
        else
            radio.Stop();
    }
    */

    // Changes the current station to the next one
    public void CycleStation()
    {
        // can't toggle the radio if it's off
        // if (!isActive)
        //     return;
        // Check if timer needs to be reset
        HandleTimer();
        // Stop current playing
        radio.Stop();
        // Change the radio station
        activeIndex = (activeIndex + 1) % unlockedStations.Count;
        // We don't need this anymore since we have a different way of muting
        if (activeIndex == 0)
        {
            radio.Stop();
            return;
        }
        // Play the static
        radio.PlayOneShot(radioStatic);
        // Change the active clip
        radio.clip = unlockedStations[activeIndex];
        // Change the clip's time to be the current timer modded by its own length
        radio.time = radioTimer % radio.clip.length;
        // Delay the radio playing by the length of the static clip
        radio.PlayDelayed(radioStatic.length);


    }    

    /// <summary>
    /// Takes a name of the station to unlock. Adds that station to the list of unlocked stations
    /// </summary>
    /// <param name="name">The name of the station to unlock</param>
    public void UnlockStation(string name)
    {
        // Check master key to see if the station doesn't exist OR it's already unlocked
        if (!MasterKey.ContainsKey(name) || MasterKey[name] == true)
            return;     // Exit early
        // Now that we know this station is valid, unlock it.
        unlockedStations.Add(DictOfStations[name]);
        // Update the key to match
        MasterKey[name] = true;
    }
}
