namespace Whodunit.app.Models
{

    // Namespaces.
    using System;
    using System.Runtime.Serialization;


    /// <summary>
    /// The request model when generating a CSV of history information.
    /// </summary>
    public class GetHistoryModel
    {

        #region Properties

        /// <summary>
        /// The start date to get history information for.
        /// </summary>
        [DataMember(Name = "startDate")]
        public DateTime? StartDate { get; set; }


        /// <summary>
        /// The end date to get history information for.
        /// </summary>
        [DataMember(Name = "endDate")]
        public DateTime? EndDate { get; set; }

        #endregion

    }

}