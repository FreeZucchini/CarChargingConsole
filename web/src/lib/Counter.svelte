<script>
  let state = $state(false)

  function toggle() {
    state = !state;
  }
</script>

<button type="button" class="counter" onclick={toggle}>{state ? "Charging" : "Not Charging"}</button>
