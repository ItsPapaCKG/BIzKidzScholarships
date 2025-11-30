import TaskList from "./TaskList";
import UserPoints from "./UserPoints"
import UserProfile from "./UserProfile";


function Dashboard() {
    return (
      <>
        <div className="dashboard">
          <div className="row">
            <h1>Welcome, Grant!</h1>
          </div>

          <div className="row g-4">
            <div className="col-8">
              <UserProfile />
            </div>
            
            <div className="col-4">
              <UserPoints/>
            </div>
          </div>
          
          
          <div className="row mt-4">
            <div className="col col-lg-6 col-xs-12">
              <TaskList />
            </div>
            
          </div>
          
        </div>
        </>
  );
}

export default Dashboard;