using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class InventoryPanelController : MonoBehaviour
{

    public UI_AbilityPanel activePanel;
    public static event EventHandler OnPanelClicked;
    public InventoryObject currentInventory;
    public InventoryObject wordrobeInventory;
    public Transform wordrobeInventoryTransform;
    private static Animator wordrobeAnimator;
    private bool isWordrobeOpen;
    [StringInList(typeof(PropertyDrawerHelper), "AllInventories")]
    public string wardrobeInventoryName;
    [SerializeField] private GameObject buyButton;
    [SerializeField] private GameObject hideToWardrobeButton;
    [SerializeField] private GameObject useButton;

    [SerializeField] private GameObject leftArrow;
    [SerializeField] private GameObject rightArrow;
   



  
    private void HideButtons()
    {

        buyButton.SetActive(false);
        hideToWardrobeButton.SetActive(false);
        useButton.SetActive(false);

    }


    private void Awake()
    {
        wordrobeAnimator = wordrobeInventoryTransform.GetComponent<Animator>();
        isWordrobeOpen= false;  

         if(!wordrobeInventory)
            wordrobeInventory = PickupItemAssets.Instance.inventoryDatabase.GetInventoryObjectFromName(wardrobeInventoryName);


       
        HideButtons();
    }


    public void TweenWardrobe() {

        isWordrobeOpen = wordrobeAnimator.GetBool("isOpen");

        if (isWordrobeOpen) 
        { 
            CloseWardrobe();
            StartHero(activePanel);
        }
        else 
        {
            OpenWardrobe();
            SwitchButtons("Wardrobe");
            ShowWardrobe();
                
                
         }


    }

    private void SwitchButtons(string panel)
    {
        HideButtons();
        switch (panel)
        {
            case "UI_Unit_Display_Hero":
                hideToWardrobeButton.SetActive(true);
                break;
            case "Wardrobe":
                useButton.SetActive(true);      
                break;
            default:
                buyButton.SetActive(true);
                break;
        }
    }


    public static void OpenWardrobe() 
    {
        wordrobeAnimator.SetBool("isOpen",true);

    
    }

    public static void CloseWardrobe()
    {
        wordrobeAnimator.SetBool("isOpen", false);

    }

    private void Start()
    {

       if(activePanel)
        StartHero(activePanel);
    }
  
   
    //public static event EventHandler HeroPanel;


    public void StartHero(UI_AbilityPanel _panel)
    {
       
        StartCoroutine(StartHeroRoutine(_panel));
    }

    IEnumerator StartHeroRoutine(UI_AbilityPanel _panel)
    {
        
        if (activePanel != null && !activePanel.name.Equals(_panel.name)&& activePanel.isPanelActive() )
        {
            activePanel.UnFillRing(0.5f);
            yield return new WaitForSeconds(0.5f);
            
        }
        isWordrobeOpen = wordrobeAnimator.GetBool("isOpen");

        if (isWordrobeOpen)
            CloseWardrobe();
        _panel.TweenRing();
        activePanel = _panel;
        currentInventory = activePanel.GetInventoryObject();

        if (currentInventory)// && !currentInventory.inventory.CheckAllInventorySlotsEmpty())
        {
            
            SwitchButtons(activePanel.name);
            OnPanelClicked?.Invoke(currentInventory, EventArgs.Empty);
        }
        else
        {
            HideButtons();
        }
        yield return null;
    }

    public void ShowWardrobe() {

        StartCoroutine(StartWardrobeRoutine());

    }

    
    
    IEnumerator StartWardrobeRoutine()
    {

        if (activePanel != null && activePanel.isPanelActive())
        {
            activePanel.UnFillRing(0.5f);
            yield return new WaitForSeconds(0.5f);

        }


        currentInventory = wordrobeInventory;
        if (currentInventory)
            OnPanelClicked?.Invoke(currentInventory, EventArgs.Empty);
        yield return null;
    }


    // Start is called before the first frame update

}
