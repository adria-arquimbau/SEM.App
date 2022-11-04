namespace SportEventsManagement.Api.Policies
{
    public static class RoleConstants
    {
        public static string User => "SEM1:SPORT_EVENTS_MANAGEMENT_USER";
        public static string Organizer => "SEM2:SPORT_EVENTS_MANAGEMENT_ORGANIZER";
        public static string Admin => "SEM3:SPORT_EVENTS_MANAGEMENT_ADMIN";
        public static string All => "SEM3:SPORT_EVENTS_MANAGEMENT_ADMIN, SEM2:SPORT_EVENTS_MANAGEMENT_ORGANIZER, SEM1:SPORT_EVENTS_MANAGEMENT_USER";
        public static string OrganizerAndAdmin => "SEM3:SPORT_EVENTS_MANAGEMENT_ADMIN, SEM2:SPORT_EVENTS_MANAGEMENT_ORGANIZER";
    }
}       