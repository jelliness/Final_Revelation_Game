<?php // this php file is used if the player came back after saving the progress (current player progress will be retrieved in a form of array)
	// this php file can also be used to check if there is a row existing in player table for playerSaveInsert and playerSaveUpdate

	include_once('connects.php');
	$player_id = $_POST['player_id'];
	$query = "SELECT * FROM Player WHERE player_id = '$player_id'";

	$check=mysqli_query($con,$query);
	$row=mysqli_num_rows($check);


	if($check == FALSE) { 
		echo ".".$row."."; // TODO: better error handling
	}

	if($row > 0)
	{
		$rownum = 0;
		while($row=mysqli_fetch_array($check))
		{
			$data[] = $row;	
			$getElements[] = $data[$rownum][1];
			$getElements[] = $data[$rownum][2];
			$getElements[] = $data[$rownum][3];
			$getElements[] = $data[$rownum][4];
			$getElements[] = $data[$rownum][5];
			++$rownum;
		}
		$playerdeets = implode("/", $getElements);
		echo $playerdeets;
	}
	else 
	{
        echo "No rows found.";
	}
?>