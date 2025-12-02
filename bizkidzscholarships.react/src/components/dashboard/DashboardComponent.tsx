import { useNavigate } from "react-router-dom";
import { UseUserAccountContext } from "../../contexts/UserAccountContext";
import TaskList from "./TaskList";
import UserPoints from "./UserPoints"
import UserProfile from "./UserProfile";
import { useEffect } from "react";


function Dashboard() {
  const context = UseUserAccountContext();
  const cookie = context.userCookie; 
  const navigate = useNavigate();

  useEffect(() => {
    if (!cookie.roles.includes("Kid")) {
            navigate("/access-denied")
        }

  }, [cookie])  
  
  return (
      <>
        <div className="dashboard">
          <div className="row">
            <h1>Welcome, Grant!</h1>
          </div>

          <div className="row g-4">
            <div className="col-12 col-md-8">
              <UserProfile />
            </div>
            
            <div className="col-12 col-md-4">
              <UserPoints/>
            </div>
          </div>
          
          
          <div className="row mt-4">
            <div className="col">
              <TaskList />
            </div>
            
          </div>
          
        </div>
        </>
  );
}

export default Dashboard;