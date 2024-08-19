using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class CutsceneActionController : MonoBehaviour
{
    [SerializeField]
    private Animator m_CutsceneAnimator;


    [SerializeField]
    private string m_CutsceneActionCSVFile;
    public struct CutsceneAction
    {
        //THESE SHOULD NOT CONTAIN COMMAS
        public string? m_ActorName;
        public string? m_Image;
        public string? m_Animation; //loop animation(?)
        public string? m_ActionFunction; //StartCoroutine use(?)

        //THE ONLY FIELD THAT MIGHT CONTAIN COMMAS
        public string? m_SpeechText;
    }
    private List<CutsceneAction> m_CutsceneActionList = new List<CutsceneAction>();
    private int m_ActionIdx = 0;
    public CutsceneAction CurrentAction
    {
        get
        {
            if (m_ActionIdx >= m_CutsceneActionList.Count)
            {
                return new CutsceneAction();
            }
            return m_CutsceneActionList[m_ActionIdx];
        }
    }
    private void Awake()
    {
        var CutscenePath = Path.Combine(Application.streamingAssetsPath, $"{m_CutsceneActionCSVFile}.csv");
        var CutsceneContents = File.ReadAllText(CutscenePath);
        if (string.IsNullOrEmpty(CutsceneContents))
        {
            Debug.LogWarning($"Cutscene file '{m_CutsceneActionCSVFile}.csv' does not exist or is empty!");
            GameObject.Destroy(this.gameObject);
            return;
        }

        foreach (var CSVLine in CutsceneContents.Split(System.Environment.NewLine, System.StringSplitOptions.None))
        {
            CutsceneAction NCutAct = new CutsceneAction();
            var CommaList = 
                CSVLine.Select((ch, idx) => new { ch, idx })
                .Where(el => el.ch == ',')
                .Select(el => el.idx)
                .ToArray();

            //only first four commas SHOULD separate csv elements
            //the rest is possible text for the character
            for (int i = 0; i < 5; ++i)
            {
                switch(i)
                {
                    case 0: //CutsceneAction ActorName
                        {
                            NCutAct.m_ActorName = CSVLine.Substring(0, CommaList[i]);
                        }
                        break;
                    case 1: //CutsceneAction Image
                        {
                            NCutAct.m_Image = CSVLine.Substring(CommaList[i - 1] + 1, CommaList[i] - CommaList[i - 1]);
                        }
                        break;
                    case 2: //CutsceneAction Animation
                        {
                            NCutAct.m_Animation = CSVLine.Substring(CommaList[i - 1] + 1, CommaList[i] - CommaList[i - 1]);
                        }
                        break;
                    case 3: //CutsceneAction Function
                        {
                            NCutAct.m_ActionFunction = CSVLine.Substring(CommaList[i - 1] + 1, CommaList[i] - CommaList[i - 1]);
                        }
                        break;
                    case 4: //CutsceneAction SpeechText
                        {
                            NCutAct.m_SpeechText = CSVLine.Substring(CommaList[i - 1] + 1);
                            NCutAct.m_SpeechText.Trim('"');
                        }
                        break;


                    default: {} break;
                }
            }
        }
    }


    public delegate void CutsceneActionDelegate();
    public void InvokeAction(CutsceneActionDelegate p_Action)
    {
        m_CutsceneAnimator.speed = 0;
        p_Action?.Invoke();
    }
    public void ResumeCutscene()
    {
        m_CutsceneAnimator.speed = 1;
    }
}
