using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Below comments written by Jonathan Sands 12/17/2022 @ 11:15AM
// ~
// TODO: Make work with Audio System when complete.  Add persistence with File I/O
// Once menu interactable menu is created (my job?) make radio work with that systm
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

    public bool radioOn;


    // WIll need a saved key of some sort to keep track of unlocked stations
    // Start is called before the first frame update
    void Start()
    {
        // Initialize collections
        DictOfStations = new Dictionary<string, AudioClip>();
        MasterKey = new Dictionary<string, bool>();
        unlockedStations = new List<AudioClip>();
        //Radio starts as off
        radioOn = false;

        // populate the ListOfStations dictionary from the MasterListOfStations
        foreach (AudioClip clip in MasterListOfStations)
        {
            DictOfStations.Add(clip.name, clip);        // Use the clip's name as the key
            MasterKey.Add(clip.name, clip);
            MasterKey[clip.name] = false;
        }
        
        // first station of unlockStations is always static
        unlockedStations.Add(radioStatic);
        // Next unlock the first 3 radio stations in the master list
        // This is temporary - This will be controlled by a save file of some sort in the future
        for (int i = 0; i < 3; i++)
        {
            // Unlock the station
            UnlockStation(MasterListOfStations[i].name);
        }
        
        
    }

    // Update is called once per frame
    void Update()
    {
        if (radioOn)
        {

            if (activeIndex != 0)
            {
                if (!radio.isPlaying)
                {
                    radio.clip = unlockedStations[activeIndex];
                    radio.Play();
                }
            }
            else
            {
                if (radio.isPlaying)
                    radio.Stop();
            }
        }

        // This is TEMPORARY to show how unlocking a new station would work.
        // In the future, the method will be queried by a separate script and will pass the name to unlock here.
        if (Input.GetKeyDown(KeyCode.U))
        {
            UnlockStation("Track 04");
        }

    }

    private void OnMouseDown()
    {
        radio.Stop();
        // Change the radio station
        radio.PlayOneShot(radioStatic);
        activeIndex = (activeIndex + 1) % unlockedStations.Count;
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

    // Toggles the radio.  Called by clicking the UI button for the radio.
    public void ToggleRadio()
    {
        radioOn = !radioOn;
        radio.Stop();
    }    
}
