using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class CutsceneActionController : MonoBehaviour
{
    [System.Serializable]
    public struct ActorResources
    {
        [System.Serializable]
        public struct ImageDescription
        {
            public string m_ImageName;
            public Sprite m_ImageSprite;
        }
        [System.Serializable]
        public struct AnimationDescription
        {
            public string m_AnimationName;
            public Animation m_Animation;
        }


        public string m_Name;
        public ImageDescription[] m_Images;
        public AnimationDescription[] m_Animations;
        public GameObject m_ActorObj;
        public bool IsEmpty
        {
            get
            {
                return string.IsNullOrEmpty(m_Name) &&
                       (m_Images == null     || m_Images.Length == 0) &&
                       (m_Animations == null || m_Animations.Length == 0);
            }
        }

        public Sprite GetSprite(string p_ImageName)
        {
            if (string.IsNullOrEmpty(p_ImageName))
            {
                return null;
            }

            foreach (var ID in m_Images)
            {
                if (ID.m_ImageName.ToLower() != p_ImageName.ToLower())
                {
                    continue;
                }
                return ID.m_ImageSprite;
            }
            return null;
        }
        public Animation GetAnimation(string p_AnimationName)
        {
            if (string.IsNullOrEmpty(p_AnimationName))
            {
                return null;
            }

            foreach (var AD in m_Animations)
            {
                if (AD.m_AnimationName.ToLower() != p_AnimationName.ToLower())
                {
                    continue;
                }
                return AD.m_Animation;
            }
            return null;
        }
    }
    [SerializeField]
    private ActorResources[] m_ActorDescriptions;
    private ActorResources GetActor(string p_ActorName)
    {
        if (string.IsNullOrEmpty(p_ActorName))
        {
            return new ActorResources();
        }

        foreach (var AD in m_ActorDescriptions)
        {
            if (AD.m_Name.ToLower() != p_ActorName.ToLower())
            {
                continue;
            }
            return AD;
        }
        return new ActorResources();
    }



    [SerializeField]
    private Animator m_CutsceneAnimator;


    [SerializeField]
    private string m_CutsceneActionCSVFile;
    public struct CutsceneAction
    {
        //THESE SHOULD NOT CONTAIN COMMAS
        public string m_ActorName; //IF YOU NEED AN ACTION WITHOUT ACTOR USE NAME "GENERAL"
        public string m_Image;
        public string m_Animation;
        public string m_ActionFunction; //StartCoroutine use(?)

        //THE ONLY FIELD THAT MIGHT CONTAIN COMMAS
        public string m_SpeechText;

        public bool IsEmpty
        {
            get
            {
                return string.IsNullOrEmpty(m_ActorName) &&
                       string.IsNullOrEmpty(m_Image) &&
                       string.IsNullOrEmpty(m_Animation) &&
                       string.IsNullOrEmpty(m_ActionFunction) &&
                       string.IsNullOrEmpty(m_SpeechText);
            }
        }
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


    [SerializeField]
    private TMPro.TextMeshProUGUI m_NameText;
    [SerializeField]
    private Image m_ImageUI;
    [SerializeField]
    private TMPro.TextMeshProUGUI m_SpeechText;
    private ActorResources m_CurrentActor;
    public void PlayNextAction()
    {
        if (CurrentAction.IsEmpty)
        {
            return;
        }

        var ActRes = GetActor(CurrentAction.m_ActorName);
        if (!ActRes.IsEmpty)
        {
            m_CurrentActor = ActRes;
        }

        //FEEL FREE TO ADD MORE CASES IF SOME ACTORS NEED TO DO SOMETHING SPECIAL
        switch(m_CurrentActor.m_Name.ToLower())
        {
            case "general":
                {
                    //HIDE ALL UI => THIS ACTION IS FOR NO ACTOR
                }
                break;


            default:
                {
                    if (m_NameText != null)
                    {
                        m_NameText.text = m_CurrentActor.m_Name;
                    }
                    if (m_ImageUI != null && !string.IsNullOrEmpty(CurrentAction.m_Image))
                    {
                        m_ImageUI.sprite = m_CurrentActor.GetSprite(CurrentAction.m_Image);
                    }
                    if (m_SpeechText != null && !string.IsNullOrEmpty(CurrentAction.m_SpeechText))
                    {
                        m_SpeechText.text = CurrentAction.m_SpeechText;
                    }

                    if (!string.IsNullOrEmpty(CurrentAction.m_ActionFunction))
                    {
                        StartCoroutine(CurrentAction.m_ActionFunction);
                    }

                    if (!string.IsNullOrEmpty(CurrentAction.m_Animation))
                    {
                        m_CurrentActor.m_ActorObj.GetComponent<Animator>()?.Play(m_CurrentActor.GetAnimation(CurrentAction.m_Animation).name);
                    }
                }
                break;
        }
    }
}
