// No idea. My guess is this is what lets GameManager.cs initialize variables of this type.
[System.Serializable]

// Class for storing Flanker Task trials ('Questions')
public class Question
{
    // Variables for the Question class
    public string flankerArrows;
    public int trialNumber;
    public bool isCongruent;
    public bool isLeft;
    public bool isPrev;
}
