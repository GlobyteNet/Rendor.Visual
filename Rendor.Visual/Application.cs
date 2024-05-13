namespace Rendor.Visual
{

    public abstract class Application
    {
        /// <summary>
        /// Called each time the application needs to render.
        /// </summary>
        public abstract void OnRender();

        /// <summary>
        /// Called when the application does not have any events to process.
        /// </summary>
        public abstract void OnIdle();
    }
}
