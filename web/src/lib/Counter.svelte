<script>
    import { onMount } from "svelte";

  let charging = $state(false)

  async function toggle() {
    const endpoint = charging ? "charge/stop" : "charge/start"; 
    const API_BASE = "https://cardashboardbackend-arg8cmfgf6befkhd.westus3-01.azurewebsites.net/api";
    try {
        const response = await fetch(`${API_BASE}/${endpoint}`, {
        method: "POST"
      });
        if (!response.ok) {
          throw new Error(`Request failed: ${response.status}`);
        }

        charging = !charging;
        console.log(`${endpoint} succeeded`);
        localStorage.setItem("charging", charging.toString());
    } catch (err) {
        console.log(`${endpoint} failed: `, err);
    }
  }

  onMount(() => {
    const storedCharging = localStorage.getItem("charging");
    charging = storedCharging === "true";

    function handleStorage(event) {
      if (event.key === "charging") {
        charging = storedCharging === "true";
      }
    }

    window.addEventListener("storage1", handleStorage);

    return () => {
      window.removeEventListener("storage1", handleStorage);
    }
  });
  
      
</script>

<button type="button" class="counter" onclick={toggle}>{charging ? "Charging" : "Not Charging"}</button>
