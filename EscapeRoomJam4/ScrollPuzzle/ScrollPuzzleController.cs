using System.Linq;
using UnityEngine;

namespace EscapeRoomJam4.ScrollPuzzle;

public class ScrollPuzzleController : Puzzle
{
    public static NomaiTextLine Red { get; private set; }
    public static NomaiTextLine Green { get; private set; }
    public static NomaiTextLine Blue { get; private set; }

    public static ScrollSocket redSocket, greenSocket, blueSocket;

    private ScrollItem _redScroll, _greenScroll, _blueScroll;

    public void Start()
    {
        transform.Find("RedWhiteboard/Props_NOM_Whiteboard (1)/whiteboard_scrollTrim").GetComponent<MeshRenderer>().material.color = new Color(1, 0.65f, 0f);
        transform.Find("GreenWhiteboard/Props_NOM_Whiteboard (1)/whiteboard_scrollTrim").GetComponent<MeshRenderer>().material.color = Color.green;
        transform.Find("BlueWhiteboard/Props_NOM_Whiteboard (1)/whiteboard_scrollTrim").GetComponent<MeshRenderer>().material.color = Color.blue;

        Red = transform.Find("RedScroll/NomaiWallText/Arc 1 - Child of -1").GetComponent<NomaiTextLine>();
        Green = transform.Find("GreenScroll/NomaiWallText/Arc 1 - Child of -1").GetComponent<NomaiTextLine>();
        Blue = transform.Find("BlueScroll/NomaiWallText/Arc 1 - Child of -1").GetComponent<NomaiTextLine>();

        _redScroll = transform.Find("RedScroll").GetComponent<ScrollItem>();
        _greenScroll = transform.Find("GreenScroll").GetComponent<ScrollItem>();
        _blueScroll = transform.Find("BlueScroll").GetComponent<ScrollItem>();

        redSocket = transform.Find("RedWhiteboard").GetComponentInChildren<ScrollSocket>();
        greenSocket = transform.Find("GreenWhiteboard").GetComponentInChildren<ScrollSocket>();
        blueSocket = transform.Find("BlueWhiteboard").GetComponentInChildren<ScrollSocket>();

        redSocket.OnSocketableDonePlacing += ScrollInserted;
        greenSocket.OnSocketableDonePlacing += ScrollInserted;
        blueSocket.OnSocketableDonePlacing += ScrollInserted;

        Solved.AddListener(() =>
        {
            redSocket.EnableInteraction(false);
            greenSocket.EnableInteraction(false);
            blueSocket.EnableInteraction(false);
        });
    }

    private void ScrollInserted(OWItem socketable)
    {
        CheckIfSolved();
    }

    public override bool IsSolved()
    {
        return redSocket._socketedItem == _redScroll && greenSocket._socketedItem == _greenScroll && blueSocket._socketedItem == _blueScroll;
    }
}
