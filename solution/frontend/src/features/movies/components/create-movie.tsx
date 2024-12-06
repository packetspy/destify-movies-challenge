/* eslint-disable @typescript-eslint/no-explicit-any */
import { Plus } from 'lucide-react'

import { Button } from '@/components/ui/button'
import { Form, FormDrawer, Input, Select, Textarea } from '@/components/ui/form'
import { useNotifications } from '@/components/ui/notifications'

import { createMovieInputSchema, useCreateMovie } from '../api/create-movie'
import { useState } from 'react'
import { useAllActors } from '../../actors/api/get-actors'
import { useAllDirectors } from '../../directors/api/get-directors'
import { Actor, Director, Rating } from '@/types/api'
import { X } from 'lucide-react'
import { updateRatingInputSchema } from '../api/update-movie'

export const CreateMovie = () => {
  const { addNotification } = useNotifications()
  const actorAllQuery = useAllActors({})
  const directorsAllQuery = useAllDirectors({})

  const createMovieMutation = useCreateMovie({
    mutationConfig: {
      onSuccess: () => {
        addNotification({
          type: 'success',
          title: 'Movie Created'
        })
      }
    }
  })

  const [reactiveActors, setReactiveActors] = useState<Actor[]>([])
  const [reactiveDirectors, setReactiveDirectors] = useState<Director[]>([])
  const [reactiveRatings, setReactiveRatings] = useState<Rating>([])

  const [selectedActor, setSelectedActor] = useState<string | null>(null)
  const [selectedDirector, setSelectedDirector] = useState<string | null>(null)

  const addActor = (uniqueId: string) => {
    if (uniqueId) {
      const actor = actorAllQuery.data?.find((actor: Actor) => actor.uniqueId === uniqueId)
      const actorExists = reactiveActors.find((actor) => actor.uniqueId === uniqueId)
      if (actor && !actorExists) setReactiveActors((prevActors) => [...prevActors, actor])
    }
  }

  const addDirector = (uniqueId: string) => {
    if (uniqueId) {
      const director = directorsAllQuery.data?.find(
        (director: Director) => director.uniqueId === uniqueId
      )
      const directorExists = reactiveDirectors.find((director) => director.uniqueId === uniqueId)
      if (director && !directorExists)
        setReactiveDirectors((prevActors) => [...prevActors, director])
    }
  }

  const addRating = (source: string, value: string) => {
    if (source && value) {
      const rating: Rating = { source: source, value: value }
      setReactiveRatings((prevActors) => [...prevActors, rating])
    }
  }

  const removeActor = (uniqueId: string) => {
    setReactiveActors((prevActors) => prevActors.filter((actor) => actor.uniqueId !== uniqueId))
  }

  const removeDirector = (uniqueId: string) => {
    setReactiveDirectors((prevDirectors) =>
      prevDirectors.filter((director) => director.uniqueId !== uniqueId)
    )
  }

  const removeRating = (source: string, value: string) => {
    setReactiveRatings((prevRatings) =>
      prevRatings.filter((rating) => rating.source !== source && rating.value !== value)
    )
  }

  return (
    <FormDrawer
      isDone={createMovieMutation.isSuccess}
      triggerButton={
        <Button size="sm" icon={<Plus className="size-4" />}>
          Create Movie
        </Button>
      }
      title="Create Movie"
      submitButton={
        <Button form="create-movie" type="submit" size="sm">
          Submit
        </Button>
      }
    >
      <Form
        id="create-movie"
        onSubmit={(values) => {
          values.actors = reactiveActors
          values.directors = reactiveDirectors
          values.ratings = reactiveRatings
          createMovieMutation.mutate({ data: values })
        }}
        schema={createMovieInputSchema}
      >
        {({ register, formState }) => (
          <>
            <Input
              label="Title"
              error={formState.errors['title']}
              registration={register('title')}
            />

            <Input
              label="Year"
              error={formState.errors['year']}
              registration={register('year')}
              type="number"
            />

            <Input
              label="Rated"
              error={formState.errors['rated']}
              registration={register('rated')}
            />

            <Input
              label="Genre"
              error={formState.errors['genre']}
              registration={register('genre')}
            />

            <Input
              label="Language"
              error={formState.errors['language']}
              registration={register('language')}
            />

            <Input
              label="Country"
              error={formState.errors['country']}
              registration={register('country')}
            />

            <Input
              label="Poster"
              error={formState.errors['poster']}
              registration={register('poster')}
            />

            <Textarea
              label="Plot"
              error={formState.errors['plot']}
              registration={register('plot')}
            />

            <div className="grid grid-cols-2 gap-4 content-end">
              <Select
                label="Actors"
                className="border border-gray-300 rounded-md"
                defaultValue={null}
                options={(actorAllQuery?.data ?? []).map((actor: Actor) => ({
                  value: actor.uniqueId,
                  label: actor.name
                }))}
                registration={register('actors' as unknown as any, {
                  onChange: (e) => setSelectedActor(e.target.value)
                })}
              ></Select>
              <div className="content-end">
                <Button
                  type="button"
                  icon={<Plus className="size-4" />}
                  onClick={() => selectedActor && addActor(selectedActor)}
                >
                  Add
                </Button>
              </div>
            </div>

            <ul>
              {reactiveActors.map((actor, index) => (
                <li key={index} className="ml-5 text-gray-700 list-none flex ">
                  {actor.name}
                  <Button
                    type="button"
                    onClick={() => removeActor(actor.uniqueId)}
                    className="content-center size-1 m-1"
                  >
                    <X className="size-4" />
                  </Button>
                </li>
              ))}
            </ul>

            <div className="grid grid-cols-2 gap-4 content-end">
              <Select
                registration={register('directors' as unknown as any, {
                  onChange: (e) => setSelectedDirector(e.target.value)
                })}
                defaultValue={null}
                label="Directors"
                className="border border-gray-300 rounded-md"
                options={(directorsAllQuery?.data ?? []).map((director: Actor) => ({
                  value: director.uniqueId,
                  label: director.name
                }))}
              ></Select>
              <div className="content-end">
                <div className="content-end">
                  <Button
                    type="button"
                    icon={<Plus className="size-4" />}
                    onClick={() => selectedDirector && addDirector(selectedDirector)}
                  >
                    Add
                  </Button>
                </div>
              </div>
            </div>

            <ul>
              {reactiveDirectors.map((director, index) => (
                <li key={index} className="ml-5 text-gray-700 list-none flex ">
                  {director.name}
                  <Button
                    type="button"
                    onClick={() => removeDirector(director.uniqueId)}
                    className="content-center size-1 m-1"
                  >
                    <X className="size-4" />
                  </Button>
                </li>
              ))}
            </ul>
          </>
        )}
      </Form>

      <Form
        id="insert-rating"
        onSubmit={(values: Rating) => {
          addRating(values.source, values.value)
        }}
        schema={updateRatingInputSchema}
      >
        {({ register, formState }) => {
          return (
            <>
              <div className="border-t-2 border-gray-700 pt-2">
                <label className="text-sm font-medium leading-none peer-disabled:cursor-not-allowed peer-disabled:opacity-70">
                  Ratings
                </label>
                <div className="grid grid-cols-3 gap-4 content-end flex">
                  <Input
                    label="Source"
                    error={formState.errors['source']}
                    registration={register('source')}
                  />

                  <Input
                    label="Value"
                    error={formState.errors['value']}
                    registration={register('value')}
                  />
                  <div className="content-end">
                    <Button type="submit" icon={<Plus className="size-4" />}>
                      Add
                    </Button>
                  </div>
                </div>

                <ul className="mt-2">
                  {reactiveRatings.map((rating, index) => (
                    <li key={index} className="ml-5 text-gray-700 list-none flex ">
                      {rating.source} - {rating.value}
                      <Button
                        type="button"
                        onClick={() => removeRating(rating?.source, rating.value)}
                        className="content-center size-1 m-1"
                      >
                        <X className="size-4" />
                      </Button>
                    </li>
                  ))}
                </ul>
              </div>
            </>
          )
        }}
      </Form>
    </FormDrawer>
  )
}
