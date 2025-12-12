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

    if (!context.isAuthenticated)
    {
      navigate("/login")
    }

  }, [cookie])
  
  useEffect(() => {
    if (!context.isAuthenticated)
    {
      navigate("/login")
    }
  }, [])
  
  return (
      <>
        <div className="container">
          <div className="row">
            <h1>Welcome, Grant!</h1>
          </div>

          <div className="row g-4">
            <div className="col-12 col-md-4 order-1 order-md-2">
              <UserPoints/>
            </div>

            <div className="col-12 col-md-8 order-2 order-md-1">
              <UserProfile />
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