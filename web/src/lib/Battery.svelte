<script>
  import {onMount} from 'svelte'; 
  let { charge = 0 } = $props()

  async function getStatusFromCar() {
    const API_BASE = "https://cardashboardbackend-arg8cmfgf6befkhd.westus3-01.azurewebsites.net/api";

    try {
      const response = await fetch(`${API_BASE}/status`, {
        method: "GET"
      });
      if (!response.ok) {
        throw new Error(`Request failed: ${response.status}`);
      }
      const data = await response.json();
      charge = data.batteryPercentage;
      console.log(`status recieved: `, data);

      // Save charge into the session storage object
      localStorage.setItem("charge", charge.toString());

    } catch (err) {
      console.log("Failed to get status:", err);
    }
  }

  onMount(() => {
    charge = Number(localStorage.getItem("charge"));

    function handleStorage(event) {
      if (event.key === "charge") {
        charge = Number(localStorage.getItem("charge"));
      }
    }

    window.addEventListener("storage2", handleStorage);
    
    const interval = setInterval(getStatusFromCar, 5000);

    return () => {
      window.removeEventListener("storage2", handleStorage);
      clearInterval(interval);
    };
  });
  
</script>

<style>
  .battery-bar {
    width: 200px;
    height: 40px;
    border: 3px solid black;
    border-radius: 3px;
    overflow: hidden;
    margin-right: 20px;
  }

  .fill {
    height: 100%;
    background: black;
    transition: width 0.3s ease;
  }
</style>

<div style="display: flex; flex-direction: row">
  <div class="battery-bar">
      <div class="fill" style={`width: ${charge}%`}></div>
  </div>
  <h2 style="font-size: 35px"> {charge}%</h2>
</div>


