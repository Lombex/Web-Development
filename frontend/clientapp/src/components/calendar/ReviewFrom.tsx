import React, { useState } from "react";

interface ReviewFormProps {
  eventId: string;
  userId: string;
  onSubmitSuccess: () => void;
}

const ReviewForm: React.FC<ReviewFormProps> = ({ eventId, userId, onSubmitSuccess }) => {
  const [rating, setRating] = useState<number>(0);
  const [feedback, setFeedback] = useState<string>("");
  const [error, setError] = useState<string | null>(null);

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();

    const review = {
      EventId: eventId,
      UserId: userId,
      Rating: rating,
      Feedback: feedback,
    };

    try {
      const response = await fetch("http://localhost:5001/api/eventattendance/review", {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify(review),
      });

      if (!response.ok) {
        throw new Error("Failed to submit review.");
      }

      setRating(0);
      setFeedback("");
      onSubmitSuccess();
    } catch (err) {
      setError("Failed to submit review. Please try again.");
    }
  };

  return (
    <form onSubmit={handleSubmit}>
      <h2 className="text-lg font-bold">Leave a Review</h2>
      {error && <p className="text-red-500">{error}</p>}
      <div>
        <label htmlFor="rating">Rating:</label>
        <input
          type="number"
          id="rating"
          value={rating}
          onChange={(e) => setRating(Number(e.target.value))}
          min="1"
          max="5"
          required
        />
      </div>
      <div>
        <label htmlFor="feedback">Feedback:</label>
        <textarea
          id="feedback"
          value={feedback}
          onChange={(e) => setFeedback(e.target.value)}
          required
        ></textarea>
      </div>
      <button type="submit">Submit Review</button>
    </form>
  );
};

export default ReviewForm;
