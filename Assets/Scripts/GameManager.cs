using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;

public enum GameStep
{
    CutsceneBegin,
    MentosMove,
    CutsceneMentosMove_MentosFall,
    MentosFall,
    CutsceneMentosFall_MentosGun,
    MentosGun,
    СutsceneMentosGun_Toaster,
    Toaster,
    CutsceneToaster_Cockroach,
    Cockroach,
    CutsceneCockroach_Plant,
    Plant,
    CutscenePlant_Bee,
    Bee,
    CutsceneBee_Computer,
    Computer
}

public class GameManager : MonoBehaviour
{
    public GameStep gameStep;
    [Space] public PlayableDirector cutsceneBegin;

    [InspectorName("CutsceneMentosMove->Fall")]
    public PlayableDirector cutsceneMentosMove_MentosFall;

    [InspectorName("CutsceneMentosFall->Gun")]
    public PlayableDirector cutsceneMentosFall_MentosGun;

    [InspectorName("CutsceneMentosGun->Toaster")]
    public PlayableDirector cutsceneMentosGun_Toaster;

    [InspectorName("CutsceneToaster->Cockroach")]
    public PlayableDirector cutsceneToaster_Cockroach;

    [InspectorName("CutsceneCockroach->Plant")]
    public PlayableDirector cutsceneCockroach_Plant;

    [InspectorName("CutscenePlane->Bee")] 
    public PlayableDirector cutscenePlant_Bee;

    [InspectorName("CutsceneBee->Computer")]
    public PlayableDirector cutsceneBee_Computer;

    [Space] 
    public UnityEvent MentosMove;
    public UnityEvent MentosFall;
    public UnityEvent MentosGun;
    public UnityEvent Toaster;
    public UnityEvent Cockroach;
    public UnityEvent Plant;
    public UnityEvent Bee;
    public UnityEvent Computer;

    private void Awake()
    {
        SwitchGameStep(gameStep);
    }

    private void OnEnable()
    {
        cutsceneBegin.stopped += (ctx) => SwitchGameStep(GameStep.MentosMove);
        cutsceneMentosMove_MentosFall.stopped +=(ctx) => SwitchGameStep(GameStep.MentosFall);
        cutsceneMentosFall_MentosGun.stopped +=(ctx) => SwitchGameStep(GameStep.MentosGun);
        cutsceneMentosGun_Toaster.stopped += (ctx) => SwitchGameStep(GameStep.Toaster);
        cutsceneToaster_Cockroach.stopped += (ctx) => SwitchGameStep(GameStep.Cockroach);
        cutsceneCockroach_Plant.stopped += (ctx) => SwitchGameStep(GameStep.Plant);
        cutscenePlant_Bee.stopped += (ctx) => SwitchGameStep(GameStep.Bee);
        cutsceneBee_Computer.stopped += (ctx) => SwitchGameStep(GameStep.Computer);
    }

    private void OnDisable()
    {
        cutsceneBegin.stopped -= (ctx) => SwitchGameStep(GameStep.MentosMove);
        cutsceneMentosMove_MentosFall.stopped -=(ctx) => SwitchGameStep(GameStep.MentosFall);
        cutsceneMentosFall_MentosGun.stopped -=(ctx) => SwitchGameStep(GameStep.MentosGun);
        cutsceneMentosGun_Toaster.stopped -= (ctx) => SwitchGameStep(GameStep.Toaster);
        cutsceneToaster_Cockroach.stopped -= (ctx) => SwitchGameStep(GameStep.Cockroach);
        cutsceneCockroach_Plant.stopped -= (ctx) => SwitchGameStep(GameStep.Plant);
        cutscenePlant_Bee.stopped -= (ctx) => SwitchGameStep(GameStep.Bee);
        cutsceneBee_Computer.stopped -= (ctx) => SwitchGameStep(GameStep.Computer);
    }

    public async void SwitchGameStep(GameStep gameStep, float time = 0f)
    {
        await UniTask.WaitForSeconds(time);
        
        this.gameStep = gameStep;
        
        switch (this.gameStep)
        {
            case GameStep.CutsceneBegin:
                PlayCutscene(cutsceneBegin);
                break;
            case GameStep.MentosMove:
                MentosMove?.Invoke();
                break;
            case GameStep.CutsceneMentosMove_MentosFall:
                PlayCutscene(cutsceneMentosMove_MentosFall);
                break;
            case GameStep.MentosFall:
                MentosFall?.Invoke();
                break;
            case GameStep.CutsceneMentosFall_MentosGun:
                PlayCutscene(cutsceneMentosFall_MentosGun);
                break;
            case GameStep.MentosGun:
                MentosGun?.Invoke();
                break;
            case GameStep.СutsceneMentosGun_Toaster:
                PlayCutscene(cutsceneMentosGun_Toaster);
                break;
            case GameStep.Toaster:
                Toaster?.Invoke();
                break;
            case GameStep.CutsceneToaster_Cockroach:
                PlayCutscene(cutsceneToaster_Cockroach);
                break;
            case GameStep.Cockroach:
                Cockroach?.Invoke();
                break;
            case GameStep.CutsceneCockroach_Plant:
                PlayCutscene(cutsceneCockroach_Plant);
                break;
            case GameStep.Plant:
                Plant?.Invoke();
                break;
            case GameStep.CutscenePlant_Bee:
                PlayCutscene(cutscenePlant_Bee);
                break;
            case GameStep.Bee:
                Bee?.Invoke();
                break;
            case GameStep.CutsceneBee_Computer:
                PlayCutscene(cutsceneBee_Computer);
                break;
            case GameStep.Computer:
                Computer?.Invoke();
                break;
        }
    }

    private void PlayCutscene(PlayableDirector cutscene)
    {
        if (cutscene)
            cutscene.Play();
        else
            SwitchGameStep(gameStep + 1);
    }
}