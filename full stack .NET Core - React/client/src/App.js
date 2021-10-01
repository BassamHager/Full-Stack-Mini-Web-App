import "./App.css";
import { BrowserRouter as Router, Route, Switch } from "react-router-dom";
import Home from "./Home";
import Department from "./Department";
import Employee from "./Employee";
import Navigation from "./Navigation";

function App() {
  return (
    <Router>
      <div className="container">
        <h3 className="m-3 d-flex justify-content-center">Mini Full Web App</h3>

        <Navigation />

        <Switch>
          <Route exact path="/" component={Home} />
          <Route path="/department" component={Department} />
          <Route path="/employee" component={Employee} />
        </Switch>
      </div>
    </Router>
  );
}

export default App;
