public interface ITutorialTask
{
    void Initialize(TutorialRoomManager roomManager);
    void StartTask();
    void Cleanup();
    string GetTaskDescription();
}
