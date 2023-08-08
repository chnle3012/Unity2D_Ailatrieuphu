using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;

namespace section2
{
    [Serializable]
    public class QuestionData
    {
        public string question;
        public string answerA;
        public string answerB;
        public string answerC;
        public string answerD;
        public string correctAnswer;
    }
    public enum GameState
    {
        Home,
        Gameplay,
        Gameover
    }

    public class GameManager : MonoBehaviour
    {
        // cau hoi va dap an
        [SerializeField] private TextMeshProUGUI m_TxtQuestion;
        [SerializeField] private TextMeshProUGUI m_TxtAnswerA;
        [SerializeField] private TextMeshProUGUI m_TxtAnswerB;
        [SerializeField] private TextMeshProUGUI m_TxtAnswerC;
        [SerializeField] private TextMeshProUGUI m_TxtAnswerD;

        // mau nen o ðap an
        [SerializeField] private Image m_ImgAnswerA;
        [SerializeField] private Image m_ImgAnswerB;
        [SerializeField] private Image m_ImgAnswerC;
        [SerializeField] private Image m_ImgAnswerD;

        // mau cac button
        [SerializeField] private Sprite m_ButtonGreen;
        [SerializeField] private Sprite m_ButtonYellow;
        [SerializeField] private Sprite m_ButtonBlack;

        // cac trang thai man hinh game
        [SerializeField] private GameObject m_HomePanel, m_GamePanel, m_GameoverPanel;
        [SerializeField] private QuestionScriptableData[] m_QuestionData;
        

        // am thanh game
        [SerializeField] private AudioSource m_AudioSource;
        [SerializeField] private AudioClip m_MusicMaintheme;
        [SerializeField] private AudioClip m_SfxWrongAnswer;
        [SerializeField] private AudioClip m_SfxCorrectAnswer;

        // khoi tao cho game
        private int m_Live = 1;
        private int m_QuestionIndex;
        private GameState m_GameState;

        // Start is called before the first frame update
        void Start()
        {
            SetGameState(GameState.Home);
            m_QuestionIndex = 0;
            InitQuestion(0);
        }

        // Update is called once per frame
        void Update()
        {

        }
        public void BtnAnswer_Pressed(string pSelectedAnswer)
        {
            bool traLoiDung = false;
            if (m_QuestionData[m_QuestionIndex].correctAnswer == pSelectedAnswer)
            {
                traLoiDung = true;
                m_AudioSource.PlayOneShot(m_SfxCorrectAnswer);
                Debug.Log("cau tra loi dung");
                Invoke("NextQuestion", 3f);
            }
            else
            {
                m_Live--;
                m_AudioSource.PlayOneShot(m_SfxWrongAnswer);
                traLoiDung = false;
                Debug.Log("cau tra loi sai");
                Invoke("NextQuestion1", 3.5f);
            }

            switch (pSelectedAnswer)
            // kiem tra ðap an ða chon thuoc textbox nao và ðoi mau
            {
                case "a":
                    m_ImgAnswerA.sprite = traLoiDung ? m_ButtonGreen : m_ButtonYellow;
                    break;
                case "b":
                    m_ImgAnswerB.sprite = traLoiDung ? m_ButtonGreen : m_ButtonYellow;
                    break;
                case "c":
                    m_ImgAnswerC.sprite = traLoiDung ? m_ButtonGreen : m_ButtonYellow;
                    break;
                case "d":
                    m_ImgAnswerD.sprite = traLoiDung ? m_ButtonGreen : m_ButtonYellow;
                    break;
            }
        }

        private void NextQuestion()
        {
            if (m_QuestionIndex < m_QuestionData.Length - 1)
                m_QuestionIndex++;
            else
            {
                SetGameState(GameState.Gameover);
                m_QuestionIndex = 0;
            }
           InitQuestion(m_QuestionIndex);
        }

        private void NextQuestion1()
        {
            SetGameState(GameState.Gameover);
            m_AudioSource.Stop();
            InitQuestion(0);
            m_QuestionIndex = 0;
        }

        private void InitQuestion(int pIndex)
        {
            if(pIndex < 0 || pIndex >= m_QuestionData.Length)
            {
                return;
            }

            m_ImgAnswerA.sprite = m_ButtonBlack;
            m_ImgAnswerB.sprite = m_ButtonBlack;
            m_ImgAnswerC.sprite = m_ButtonBlack;
            m_ImgAnswerD.sprite = m_ButtonBlack;

            m_TxtQuestion.text =  m_QuestionData[pIndex].question;
            m_TxtAnswerA.text = "A. " + m_QuestionData[pIndex].answerA;
            m_TxtAnswerB.text = "B. " + m_QuestionData[pIndex].answerB;
            m_TxtAnswerC.text = "C. " + m_QuestionData[pIndex].answerC;
            m_TxtAnswerD.text = "D. " + m_QuestionData[pIndex].answerD;
        }

        public void SetGameState(GameState state)
        {
            m_GameState = state;
            m_Live = 1;
            m_HomePanel.SetActive(m_GameState == GameState.Home);
            m_GamePanel.SetActive(m_GameState == GameState.Gameplay);
            m_GameoverPanel.SetActive(m_GameState == GameState.Gameover);
        }
      
        public void BtnPlay_Pressed()
        {
            m_Live = 1;
            SetGameState(GameState.Gameplay);

            
            InitQuestion(0);
            m_AudioSource.clip = m_MusicMaintheme;
            m_AudioSource.Play();
        }
        public void BtnHome_Pressed()
        {
            SetGameState(GameState.Home);
        }
    }
}