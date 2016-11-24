namespace Izio.Umbraco.Emailer.Interfaces
{
    /// <summary>
    /// 
    /// </summary>
    public interface IDatabaseEntity
    {
        /// <summary>
        /// 
        /// </summary>
        bool IsNew { get; }

        /// <summary>
        /// 
        /// </summary>
        bool IsDirty { get; }
    }
}
