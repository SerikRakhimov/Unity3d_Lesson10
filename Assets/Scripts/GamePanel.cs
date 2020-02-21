using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePanel : MonoBehaviour
{
    [SerializeField]
    private Transform root;

    [SerializeField]
    private Game game;

    [SerializeField]
    private QuizButton quizButton;

    [SerializeField]
    private Text questionText;

    [SerializeField]
    private Transform panelParent;

    [SerializeField]
    private Dropdown dropdown;

    // текущий вопрос с ответами
    private Question question;

    // код языка (0 - RU, 1 - EN, 3 - KZ)
    private int idLanguage;

    private void Awake()
    {
	// считываем с реестра idLanguage
        idLanguage = PlayerPrefs.GetInt("idLanguage");
    	dropdown.value = idLanguage;
    }

    private void Start()
    {
        // отключаем панель на старте игры
        root.gameObject.SetActive(false);
	// обнуляем вопрос с ответами
	question = null;    }

    public void SetPanelActive(bool state)
    {
        root.gameObject.SetActive(state);
    }

    public void Init(Question ques)
    {
        // прием в класс вопроса с ответами
	question = ques;

	// отобразить вопрос с ответами
	SetUI();
	
    }

    private void SetUI()
    {
	// проверка нужна: при загрузке idLanguage с реестра значение question = null
	if (question == null)
	{
        	return;
	}

        // считать вопрос
        questionText.text = GetQuestionName();
        
	// кнопки - варианты ответов очистить у panelParent
	foreach (Transform child in panelParent)
	{
		 Destroy(child.gameObject);
	}

	// создать динамически кнопки - варианты ответов 
        foreach (var item in question.options)
        {
            var button = Instantiate(quizButton, panelParent);
            button.Init(item.id, GetOptionName(item));
            button.onClick +=ButtonPress;
        }
    }

    private string GetQuestionName()
    {
	string result = "";
 	switch (idLanguage)
      	{
          case 0:
	      result = question.name_ru;
              break;
          case 1:
	      result = question.name_eng;
              break;
          case 2:
	      result = question.name_kz;
              break;
     	}
        return result;	
    }

    private string GetOptionName(Option option)
    {
	string result = "";
	switch (idLanguage)
      	{
          case 0:
	      result = option.name_ru;
              break;
          case 1:
	      result = option.name_eng;
              break;
          case 2:
	      result = option.name_kz;
              break;
     	}
        return result;	
    }

    private void ButtonPress(int id)
    {
 
        game.SetAnswer(id);
    }

  

    public void LanguagePress(int idLang)
    {
	idLanguage = idLang;
	SetUI();
    }

    public void OnDestroy()
    {
	    // сохранить в реестре значение idLanguage
            PlayerPrefs.SetInt("idLanguage", idLanguage);
    }

}
