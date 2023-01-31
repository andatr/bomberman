namespace Bomberman
{
    public interface IBombPoolModel_Controller
    {
        IBombModel_View[] Bombs { get; }

        void Update(float deltaTime);
    }
}