

public class PointSystem {
    
    public record PointRecord(User user, int Points, DateTime Date);

    /*
    - Users are getting points for participating with events.
    - When user has a current streak you get bonus points

    - Users geting points for giving feedback on participated event.

    - When logging in on time you earn points.

    - After Finishing a event you earn bonus points.

    - You can trade points for functions such as.
        Dark theme, Levels.

    - You can earn badges or special roles with x amount of points.
    - Special roles and badges are shown on users profiles.

    × There must be a dashboard where the users progression is shown.
    - There must be a leaderboard for users to make it competetive.
   
    */

    // TODO
    // Make a function that can read and write to the database.
    // Make a fuction that (Get, Add, Delete, Update) Users points.
    
    // Make a function to trade with points for cosmetics.
    // Make a list of purchasable badges / special roles.
    // Make a list of purchasable cosmetics.

    // Make a leaderboard for users with most amount of points.

    /* -> Inherit from EventServices to check if user is participating in events.
            - Check the current streak and add x amount bonus points to users.
            - Add bonus point when feedback has been given.
            - If Event is finished you earn bonus points.
    */

    /* -> Inherit from Login {Get user time}
          Time of login/logout should be handled in the Event Attandance class.
          |
          |_ Get user login/logout time from Event Attandance class and add points accordingly. 
    */

}