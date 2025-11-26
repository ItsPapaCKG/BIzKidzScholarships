import TaskList from "./TaskList";
import UserPoints from "./UserPoints";
import UserProfile from "./UserProfile";


function Dashboard() {
    return (
      <>
        <UserProfile />
        <UserPoints/>
        <TaskList />
        </>
  );
}

export default Dashboard;