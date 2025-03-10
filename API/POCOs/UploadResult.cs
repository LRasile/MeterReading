namespace POCOs
{
    public class UploadResult
    {
        public UploadResult(int successfulRecords, int failedRecords)
        {
            SuccessfulRecords = successfulRecords;
            FailedRecords = failedRecords;
        }

        public int SuccessfulRecords { get; set; }
        public int FailedRecords { get; set; }
    }
}
