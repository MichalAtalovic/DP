namespace PubCiterAPI.Repositories.Interfaces
{
    using PubCiterAPI.Model;
    using System.Collections.Generic;

    /// <summary>
    /// QuarantineRepository Interface
    /// </summary>
    public interface IQuarantineRepository
    {
        /// <summary>
        /// Moves publication to quarantine by ID
        /// </summary>
        /// <param name="context">Application DB context instance</param>
        /// <param name="publicationId">Unique publication's system identifier</param>
        public void MoveToQuarantine(ApplicationDbContext context, long publicationId);

        /// <summary>
        /// Removes publication from quarantine by quarantined publication ID
        /// </summary>
        /// <param name="context">Application DB context instance</param>
        /// <param name="publicationId">Unique quarantined publication's system identifier</param>
        public void RemoveFromQuarantine(ApplicationDbContext context, long publicationId);

        /// <summary>
        /// Gets the list of quarantined publiations
        /// </summary>
        /// <param name="context">Application DB context instance</param>
        /// <param name="appendCitations">Whether to append citations to response</param>
        public IEnumerable<QuarantinedPublication> GetQuarantineList(ApplicationDbContext context, bool appendCitations);
    }
}
