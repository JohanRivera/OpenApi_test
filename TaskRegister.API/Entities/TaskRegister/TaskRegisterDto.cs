namespace TaskRegister.API.Entities.TaskRegister
{
    public class TaskRegisterDto
    {
        public string Subject { get; set; }
        public string UserId { get; set; }
        public string ProjectId { get; set; }
        public string AssignedTo { get; set; }
        public string TaskDescription { get; set; }
        public string? Comment { get; set; }
        public int Time { get; set; }
        public DateTime AssignedDate { get; set; }
    }

    public class DeleteTimeslipDto
    {
        public string Subject { get; set; }
    }

    public class SearchTimeslipDto
    {
        public string UserId { get; set; }
        public string ProjectId { get; set; }
        public bool IsAll { get; set; }
        public string DateLimitUp { get; set; }
        public string DateLimitDown { get; set; }
    }

    public class ReturnTimeslipsDto
    {
        public string Subject { get; set; }
        public string UserId { get; set; }
        public string ProjectId { get; set; }
        public string ProjectName { get; set; }
        public string AssignedTo { get; set; }
        public string TaskDescription { get; set; }
        public string? Comment { get; set; }
        public int Time { get; set; }
        public DateTime AssignedDate { get; set; }
    }
}
