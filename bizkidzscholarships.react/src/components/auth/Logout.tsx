import { Link } from "react-router-dom";

function Logout() {
  return (
      <>
          <div className="justify-content-center">
              <div className="row"><h1>You have logged out.</h1></div>
              <div className="row"><Link to="/login">Log Back In</Link></div>   
          </div >
        </>
  );
}

export default Logout;