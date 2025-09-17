using TMPro;
using UnityEngine;

public class InteractableView : MonoBehaviour
{
    [SerializeField] private TMP_Text _titleText;
    
    public string Title
    {
        get => _titleText.text;
        set => _titleText.text = value;
    } 
}
