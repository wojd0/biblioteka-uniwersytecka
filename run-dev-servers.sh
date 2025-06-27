#!/bin/zsh
# Script to run backend and frontend dev servers in split screen using tmux

# Set paths
BACKEND_DIR="$(cd "$(dirname "$0")/GetPapierek" && pwd)"
FRONTEND_DIR="$(cd "$(dirname "$0")/GetPapierekWeb" && pwd)"

# Kill any running dotnet processes in the backend directory to avoid port conflicts
BACKEND_PID=$(lsof -ti:5000)
if [ -n "$BACKEND_PID" ]; then
  echo "Killing existing backend process on port 5000 (PID: $BACKEND_PID)"
  kill -9 $BACKEND_PID
  sleep 1
fi

# Start tmux session
SESSION="devservers"
tmux new-session -d -s $SESSION

tmux send-keys -t $SESSION "cd $BACKEND_DIR && dotnet run" C-m
tmux split-window -h -t $SESSION

tmux send-keys -t $SESSION:0.1 "cd $FRONTEND_DIR && npm start" C-m

tmux select-pane -t $SESSION:0.0

tmux attach -t $SESSION
