<?php // this php file is used if there is no player info yet (first time saving progress)

    include_once('connects.php');
    $player_id = $_POST['player_id'];
    $player_position_x = $_POST['player_position_x'];
    $player_position_y = $_POST['player_position_y'];
    $paper_collected = $_POST['paper_collected'];
    $key_collected = $_POST['key_collected'];
    $remaining_health = $_POST['remaining_health'];

    $result = mysqli_query($con,"UPDATE Player SET player_position_x = '$player_position_x', player_position_y = '$player_position_y', paper_collected = '$paper_collected', key_collected = '$key_collected', remaining_health = '$remaining_health' WHERE player_id = '$player_id'");
    echo "Player Information Updated!";

?>