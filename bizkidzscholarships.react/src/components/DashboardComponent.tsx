import TaskList from "./TaskList";
import ImageUpload from "./tasks/ImageUpload";
import UserPoints from "./UserPoints";
import UserProfile from "./UserProfile";


function Dashboard() {
    return (
      <>
        <UserProfile />
        <UserPoints/>
        <TaskList />
        <ImageUpload />
        </>
  );
}

export default Dashboard;